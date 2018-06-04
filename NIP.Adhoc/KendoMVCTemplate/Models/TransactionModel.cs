using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZenithBankXpath.Web.Models
{
    public class TransactionModel
    {
        public string TransactionId { get; set; }
        [Required]
        [StringLength(10, MinimumLength =10, ErrorMessage ="Account Number is invalid")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Account Number is invalid")]
        [Display(Name = "Account Number")]
        public string AccountNo { get; set; }
        [Required]
        [Display(Name = "Merchant Id")]
        public string MerchantId { get; set; }
        public string Currency { get; set; }
        [Display(Name = "Available Balance")]
        public decimal AvailableBalance { get; set; }
        [Required]
        [Display(Name = "Transaction Code")]
        public string TranCode { get; set; }
        [Required]
        [Display(Name = "Transaction Type")]
        public string TranType { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(typeof(decimal), "0","79228162514264337593543950335")]
        public decimal Amount { get; set; }       
        
    }

    struct EncryptModel
    {
        public string channelId { get; set; }
        public string accountNumber { get; set; }
        public int merchantId { get; set; }
    }
}