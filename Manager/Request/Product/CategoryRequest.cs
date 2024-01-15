using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager.Request.Product
{
    public class Category1AddRequest
    {
        [Required(ErrorMessage = "대분류명을 입력하여 주세요.")]
        public string categoryName { get; set; }

        [Required(ErrorMessage = "게시여부를 선택하여 주세요.")]
        public string displayFlag { get; set; }
    }

    public class Category2AddRequest
    {
        [Required(ErrorMessage = "대분류코드를 선택하여 주세요.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "대분류코드가 잘못 되었습니다.")]
        public string categoryCode1 { get; set; }

        [Required(ErrorMessage = "소분류명을 입력하여 주세요.")]
        public string categoryName { get; set; }

        [Required(ErrorMessage = "게시여부를 선택하여 주세요.")]
        public string displayFlag { get; set; }
    }

    public class Category1ModifyRequest
    {
        [Required(ErrorMessage = "대분류코드를 선택하여 주세요.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "대분류코드가 잘못 되었습니다.")]
        public string categoryCode1 { get; set; }

        [Required(ErrorMessage = "대분류명을 입력하여 주세요.")]
        public string categoryName { get; set; }

        [Required(ErrorMessage = "게시여부를 선택하여 주세요.")]
        public string displayFlag { get; set; }
    }

    public class Category2ModifyRequest
    {
        [Required(ErrorMessage = "대분류코드가 잘못 되었습니다.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "대분류코드가 잘못 되었습니다.")]
        public string categoryCode1 { get; set; }

        [Required(ErrorMessage = "소분류코드가 잘못 되었습니다.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "소분류코드가 잘못 되었습니다.")]
        public string categoryCode2 { get; set; }

        [Required(ErrorMessage = "소분류명을 입력하여 주세요.")]
        public string categoryName { get; set; }

        [Required(ErrorMessage = "게시여부를 선택하여 주세요.")]
        public string displayFlag { get; set; }
    }

    public class Category1DisplayNumModifyRequest
    {
        [Required(ErrorMessage = "순서게시 정보가 없습니다.")]
        public string modType { get; set; }

        [Required(ErrorMessage = "대분류코드가 잘못 되었습니다.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "대분류코드가 잘못 되었습니다.")]
        public string categoryCode1 { get; set; }
    }

    public class Category2DisplayNumModifyRequest
    {
        [Required(ErrorMessage = "순서게시 정보가 없습니다.")]
        public string modType { get; set; }

        [Required(ErrorMessage = "대분류코드가 잘못 되었습니다.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "대분류코드가 잘못 되었습니다.")]
        public string categoryCode1 { get; set; }

        [Required(ErrorMessage = "소분류코드가 잘못 되었습니다.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "소분류코드가 잘못 되었습니다.")]
        public string categoryCode2 { get; set; }
    }
}