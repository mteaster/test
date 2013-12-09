﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public string Category { get; set; }

        public enum SizeEnum { XSmall, Small, Medium, Large, XLarge }

        [Display(Name = "Size")]
        public SizeEnum MerchSize { get; set; }

        public SelectList MerchSizeList { get; set; }

        public int BandId { get; set; }

        public Merchandise()
        {
            MerchSizeList = GetSizeSelectList();
        }

        public static SelectList GetSizeSelectList()
        {

            var enumValues = Enum.GetValues(typeof(Merchandise.SizeEnum)).Cast<Merchandise.SizeEnum>().Select(e => new { Value = e.ToString(), Text = e.ToString() }).ToList();

            return new SelectList(enumValues, "Value", "Text", "");
        }
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

        [Display(Name = "Date (mm/dd/yyyy)")]
        public DateTime Date { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Associated Band")]
        public int AssociatedBandContactId { get; set; }

        [Display(Name = "Associated Person")]
        public int AssociatedPersonContactId { get; set; }

        [Display(Name = "Associated Venue")]
        public int AssociatedVenueContactId { get; set; }

        public string AssociatedBandName { get; set; }
        public string AssociatedPersonName { get; set; }
        public string AssociatedVenueName { get; set; }

        [Display(Name = "Paid")]
        public Boolean Paid { get; set; }

        public int BandId { get; set; }
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

        [Display(Name = "Date (mm/dd/yyyy)")]
        public DateTime Date { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Associated Band")]
        public int AssociatedBandContactId { get; set; }

        [Display(Name = "Associated Person")]
        public int AssociatedPersonContactId { get; set; }

        [Display(Name = "Associated Venue")]
        public int AssociatedVenueContactId { get; set; }

        public string AssociatedBandName { get; set; }
        public string AssociatedPersonName { get; set; }
        public string AssociatedVenueName { get; set; }

        [Display(Name = "Paid")]
        public Boolean Paid { get; set; }

        public int BandId { get; set; }
    }

    public class IndexFilters
    {
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDT { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndDT { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Paid")]
        public bool Paid { get; set; }

        [Display(Name = "Unpaid")]
        public bool Unpaid { get; set; }
    }
}