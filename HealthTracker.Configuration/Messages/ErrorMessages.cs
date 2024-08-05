using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTracker.Configuration.Messages;
public static class ErrorMessages
{   
    public static class Generic
    {
        public static string TypeBadRequest = "Bad Request";

        public static string InvalidPayload = "Invalid Payload";
        public static string UnableToProcess = "Unable to process request";
        public static string SomethingWentWrong  = "something went wrong, plz try again";
    }

    public static class Profile
    {
        public static string UserNotFound = "User not found";
    }

}
