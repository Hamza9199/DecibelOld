/*using Decibel.Models;
using Stripe;
using Stripe.Checkout;

public class StripeService
{
	private readonly IConfiguration _configuration;

	public StripeService(IConfiguration configuration)
	{
		_configuration = configuration;
		StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
	}

	public string CreateCheckoutSession(Pretplata pretplata)
	{
		var options = new SessionCreateOptions
		{
			PaymentMethodTypes = new List<string> { "card" },
			LineItems = new List<SessionLineItemOptions>
			{
				new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						Currency = "bam",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = pretplata.naziv
						},
						UnitAmount = (long)(pretplata.cijena * 100)
					},
					Quantity = 1
				}
			},
			Mode = "payment",
			SuccessUrl = "https://localhost:5001/Pretplata/Success",
			CancelUrl = "https://localhost:5001/Pretplata/Cancel"
		};

		var service = new SessionService();
		var session = service.Create(options);
		return session.Url;
	}
}


namespace Decibel.Services
{
	

	public class StripeService
	{
		private readonly IConfiguration _configuration;

		public StripeService(IConfiguration configuration)
		{
			_configuration = configuration;
			StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
		}

		public string CreateCheckoutSession(Pretplata pretplata)
		{
			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = new List<SessionLineItemOptions>
			{
				new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						Currency = "bam",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = pretplata.naziv
						},
						UnitAmount = (long)(pretplata.cijena * 100)
					},
					Quantity = 1
				}
			},
				Mode = "payment",
				SuccessUrl = "https://localhost:5001/Pretplata/Success",
				CancelUrl = "https://localhost:5001/Pretplata/Cancel"
			};

			var service = new SessionService();
			var session = service.Create(options);
			return session.Url;
		}
	}

}*/
