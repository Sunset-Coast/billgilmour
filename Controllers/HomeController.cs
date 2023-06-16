using BillTest.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Diagnostics;


namespace BillTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Industries()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["SwalMessage"] = TempData["SwalMessage"] as string;
            var model = new ContactModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get the email settings from User Secrets
                    var emailSettings = _configuration.GetSection("EmailSettings");
                    var smtpServer = emailSettings["EmailHost"];
                    var smtpPort = Convert.ToInt32(emailSettings["EmailPort"]);
                    var username = emailSettings["EmailAddress"];
                    var password = emailSettings["EmailPassword"];

                    // Create the email message
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Your Name", "your-email@example.com"));
                    message.To.Add(new MailboxAddress("Recipient Name", "jvtdevtest17@gmail.com"));
                    message.Subject = "New Contact Form Submission";

                    // Build the email body
                    var builder = new BodyBuilder();
                    builder.TextBody = $"Name: {model.FirstName} {model.LastName}{Environment.NewLine}Email: {model.SenderEmail}{Environment.NewLine}{model.PhoneNumber}{Environment.NewLine}Message: {model.Message}";
                    message.Body = builder.ToMessageBody();

                    // Configure the email client
                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                        await client.AuthenticateAsync(username, password);

                        // Send the email
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }

                    TempData["SwalMessage"] = "Success: We have received your message!";
                    // Redirect to a confirmation page
                    return RedirectToAction("Contact");
                }
                catch (Exception)
                {
                    TempData["SwalMessage"] = "Error: Looks like something went wrong!";

                    return RedirectToAction("Index");
                    // Handle any exceptions that occurred during email sending
                    //ModelState.AddModelError("", "An error occurred while sending the email. Please try again later.");
                    // Log the exception if needed
                    throw;
                }
            }

            // If the model is not valid, return to the form view to display validation errors
            return View("Contact", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}