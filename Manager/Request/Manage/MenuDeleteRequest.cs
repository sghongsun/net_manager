using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager.Request.Manage
{
    public class MenuDeleteRequest
    {
        [Required(ErrorMessage = "올바른 값이 아닙니다.")]
        [StringLength(4, ErrorMessage = "올바른 값이 아닙니다.")]
        public string menuPCode { get; set; }

        [Required(ErrorMessage = "올바른 값이 아닙니다.")]
        [StringLength(4, ErrorMessage = "올바른 값이 아닙니다.")]
        public string menuCode { get; set; }
    }
}