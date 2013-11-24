﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace test.Models.Budget
{

    [Table("Merchandise")]
    public class Merchandise
    {
        [Key]
        [Display(Name = "Merchandise Id")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MerchandiseId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public string Name { get; set; }

        public enum SizeEnum { XSmall, Small, Medium, Large, XLarge }

        [Display(Name = "Size")]
        public SizeEnum Size { get; set; }
    }

    [Table("AccountPayables")]
    public class AccountPayables
    {
        [Key]
        [Display(Name = "AccountPayableId")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AccountPayableId { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Associated Band")]
        public int AssociatedBandContactId { get; set; }

        [Display(Name = "Associated Person")]
        public int AssociatedPersonContactId { get; set; }

        [Display(Name = "Associated Venue")]
        public int AssociatedVenueContactId { get; set; }

        [Display(Name = "Paid")]
        public Boolean Paid { get; set; }
    }

    [Table("AccountReceivables")]
    public class AccountReceivables
    {
        [Key]
        [Display(Name = "AccountReceivableId")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AccountReceivableId { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Associated Band")]
        public int AssociatedBandContactId { get; set; }

        [Display(Name = "Associated Person")]
        public int AssociatedPersonContactId { get; set; }

        [Display(Name = "Associated Venue")]
        public int AssociatedVenueContactId { get; set; }

        [Display(Name = "Paid")]
        public Boolean Paid { get; set; }
    }
}