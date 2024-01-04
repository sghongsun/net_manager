using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager.Request.Manage
{
    public class TermsSearch : Search
    {
        public TermsSearch() {
            searchtype = "title";
        }
    }

    public class TermsAddRequest
    {
        [Required(ErrorMessage = "제목을 입력하여 주세요.")]
        public string title { get; set; }

        [Required(ErrorMessage = "적용위치를 입력하여 주세요.")]
        public string place { get; set; }

        [Required(ErrorMessage = "내용을 입력하여 주세요.")]
        public string contents { get; set; }
    }

    public class TermsModifyRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "잘못된 값 입니다.")]
        public int idx { get; set; }

        [Required(ErrorMessage = "제목을 입력하여 주세요.")]
        public string title { get; set; }

        [Required(ErrorMessage = "적용위치를 입력하여 주세요.")]
        public string place { get; set; }

        [Required(ErrorMessage = "내용을 입력하여 주세요.")]
        public string contents { get; set; }
    }

    public class TermsDeleteRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "잘못된 값 입니다.")]
        public int idx { get; set; }
    }
}