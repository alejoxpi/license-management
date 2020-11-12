namespace LM.Domain
{
    public class BasicResponse    {         
       
        public string Code { get; set; }    
       
        public string Message { get; set; } 

       
        public string Status { get; set; }        
       
       
    }

    public class LicenseActivationSuccessResponse : BasicResponse
    {
        public string LicenseCode;
    }

    public class LicenseValidationResponse : BasicResponse
    {
        public int RemainingTime;
        public string LicenseStatus;
        public bool active;
        public bool valid;

    }
}