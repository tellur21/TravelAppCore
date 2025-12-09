﻿using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Stripe;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StripeWebhookHandlerService : IWebhookHandlerService
    {
        private readonly StripeSettings _stripeSettings;
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<StripeWebhookHandlerService> _logger;

        public StripeWebhookHandlerService(
            IOptions<StripeSettings> stripeSettings,
            IBookingRepository bookingRepository,
            ILogger<StripeWebhookHandlerService> logger)
        {
            _stripeSettings = stripeSettings.Value;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        public async Task ProcessWebhookEventAsync(string json, string signature)
        {
            var stripeEvent = EventUtility.ConstructEvent(json, signature, _stripeSettings.WebhookSecret);
            PaymentIntent? paymentIntent;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (paymentIntent != null && paymentIntent.Metadata.TryGetValue("booking_id", out var bookingIdStr) && Guid.TryParse(bookingIdStr, out var bookingId))
                    {
                        var booking = await _bookingRepository.GetByIdAsync(bookingId);
                        if (booking != null)
                        {
                            // Idempotency check: Only update if the status is still pending.
                            if (booking.Status == BookingStatus.Pending)
                            {
                                booking.Status = BookingStatus.Confirmed;
                                await _bookingRepository.UpdateAsync(booking);
                                _logger.LogInformation("Booking {BookingId} confirmed via Stripe webhook.", bookingId);
                            }
                            else
                            {
                                _logger.LogInformation("Webhook for booking {BookingId} received, but status is already {Status}. No action taken.", bookingId, booking.Status);
                            }
                        }
                    }
                    break;

                case "payment_intent.payment_failed":
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (paymentIntent != null && paymentIntent.Metadata.TryGetValue("booking_id", out bookingIdStr) && Guid.TryParse(bookingIdStr, out bookingId))
                    {
                        var booking = await _bookingRepository.GetByIdAsync(bookingId);
                        if (booking != null && booking.Status == BookingStatus.Pending)
                        {
                            booking.Status = BookingStatus.Cancelled;
                            await _bookingRepository.UpdateAsync(booking);
                            _logger.LogWarning("Booking {BookingId} cancelled due to payment failure. Reason: {FailureReason}", bookingId, paymentIntent.LastPaymentError?.Message);
                        }
                    }
                    break;

                default:
                    _logger.LogInformation("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                    break;
            }
        }
    }
}