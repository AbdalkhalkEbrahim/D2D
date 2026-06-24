
namespace Application.Response

{

    public sealed record CustomError(string StatusCode, Dictionary<string, string> Messages) 
    {
        public Error WithTarget(string target)
        {
            var message = Messages.TryGetValue(target, out var msg) ? msg : Messages["Default"];
            return new Error(StatusCode, message);
        }
    }

    public static class Messages

    {

        // 404 Not Found Group

        public static readonly CustomError NotFound = new(

            "NotFound",

            new Dictionary<string, string>

            {

                { "Default", "The requested resource was not found." },

                { "User", "The user could not be found." },

                { "Product", "The requested product variant or material was not found." },

                { "Design", "The design layout or blueprint was not found." },

                { "Order", "The specified order does not exist in our records." },
                {"Address", "The specified address does not exist in our records." }
            });



        // 409 Conflict Group

        public static readonly CustomError Conflict = new(

            "Conflict",

            new Dictionary<string, string>

            {

                { "Default", "The request could not be completed due to a conflict with the current state of the target resource." },

                { "Email", "This email address is already registered on our platform." },

                { "Sku", "A product with this SKU code already exists." },

                { "Design", "This design title or file has already been uploaded." },
                {"Address","Cannot delete the selected address. Please select another address before deleting this one." },
                {"Offer","This design has already a publish offer" },
                {"CustomerOffer","You've already submitted a request for this offer" }

            });



        // 400 Bad Request Group (System & Business Validation)

        public static readonly CustomError BadRequest = new(

            "BadRequest",

            new Dictionary<string, string>

            {

                { "Default", "The request could not be understood by the server due to malformed syntax." },

                { "ValidationError", "One or more validation errors occurred." },

                { "InvalidCredentials", "Invalid Email/Password." },

                { "NullValue", "The provided value cannot be null." },

                { "InvalidProductSpecs", "The provided production specifications do not match the designer's original requirements." },

                { "InvalidOrderStateTransition", "The order status cannot be updated to the requested state from its current state." },

                { "CancellationWindowClosed", "This order cannot be canceled because it has already entered the production or shipping phase." },

                { "PriceMismatch", "The final checkout price does not match the accumulated cost of items, production, and shipping." },

                { "PasswordMismatch", "The passwords provided do not match." },
                { "InvalidRequest", "The registration request parameters are invalid, do not match your account type, or your email address has not been verified yet." },
                { "Underage", "You do not meet the minimum age requirement to register on this platform." },

                { "UserCreationFailed", "Failed to create the user account in our identity management system." },
                { "PasswordChangeFailed", "Failed to update the password. Please verify your current password and try again." },
                { "ImageUploadFailed", "Failed to upload one or more images. Please try again." },
                {"Design", "The selected design maybe published or not exsisted " }
            });



        // 403 Forbidden Group (Permissions & Workflow Restrictions)

        public static readonly CustomError Forbidden = new(

            "Forbidden",

            new Dictionary<string, string>

            {

                { "Default", "You do not have permission to perform this action." },

                { "DesignNotApproved", "This design cannot be produced or purchased because it hasn't been approved by the platform yet." },

                { "InsufficientWalletBalance", "The user's wallet balance is insufficient to complete this transaction." }

            });



        // 423 Locked / Account Status Group

        public static readonly CustomError AccountStatus = new(

            "AccountStatus",

            new Dictionary<string, string>

            {

                { "Default", "This account or resource is currently unavailable." },

                { "Pending", "This account is pending approval and currently waiting to be reviewed by an administrator." },

                { "Rejected", "This account has been rejected and cannot access the platform. Please contact support for more information." },

                { "Disabled", "This account or resource is currently disabled or inactive." },

                { "Suspended", "This account has been temporarily suspended due to a violation of platform policies." }

            });



        // 401 Unauthorized

        public static readonly CustomError Unauthorized = new(

            "Unauthorized",

            new Dictionary<string, string>

            {

                { "Default", "Authentication is required to access this resource." }

            });



        // 422 Unprocessable Entity

        public static readonly CustomError UnprocessableEntity = new(

            "UnprocessableEntity",

            new Dictionary<string, string>

            {

                { "Default", "The server was unable to process the contained instructions." },

                { "OutOfStock", "The requested product variant or material is currently out of stock." }

            });



        // 408 Timeout

        public static readonly CustomError Timeout = new(

            "Timeout",

            new Dictionary<string, string>

            {

                { "Default", "The server timed out waiting for the request to complete." }

            });



        // 410 Expired

        public static readonly CustomError Expired = new(

            "Expired",

            new Dictionary<string, string>

            {

                { "Default", "The requested resource, token, or code has expired." },
                { "MagicToken", "The magic login link has expired. Please request a new one." },
                {"Otp", "The OTP code has expired. Please request a new one." },
                {"Token","Invalid, revoked or expired token" },
                
            });



        // 429 Rate Limit

        public static readonly CustomError RateLimitExceeded = new(

            "RateLimitExceeded",

            new Dictionary<string, string>

            {

                { "Default", "Too many requests have been made. Please try again later." }

            });



        // 500 Internal Server Error

        public static readonly CustomError SystemError = new(

            "SystemError",

            new Dictionary<string, string>

            {
                { "Default", "An unexpected error occurred on the server. Please try again later." },
            });
        public static Error AccountLocked(int minutes, int seconds) =>
             new("AccountStatus", $"Account is locked. Try again in {minutes} minutes and {seconds} seconds.");

        public static Error AccountLocked(int minutes) =>
            new("AccountStatus", $"Account is locked due to multiple failed login attempts. Try again in {minutes} minutes.");
        public static Error OtpBackoff(int minutes, int seconds) =>
             new("Forbidden", $"Please Try again in {minutes} minutes and {seconds} seconds.");
        public static Error CloudinaryError(string message) => new("SystemError", $"Cloudinary Error: {message}");
        public static Error EmailConnection(string message) => new("SystemError", $"Email connection Error : {message}");
    }
}