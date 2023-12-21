using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager.Request.Manage
{
    public class MenuAddRequest
    {
        [Required(ErrorMessage = "올바른 값이 아닙니다.")]
        [StringLength(4, ErrorMessage = "올바른 값이 아닙니다.")]
        public string menuPCode { get; set; }

        [Required(ErrorMessage = "메뉴명을 입력하여 주세요.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "메뉴명을 정확히 입력하여 주세요.")]
        public string menuName { get; set; }

        [Required(ErrorMessage = "메뉴 URL을 입력하여 주세요.")]
        public string menuUrl { get; set; }
    }
}