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
}