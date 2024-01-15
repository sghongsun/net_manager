using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager.Request.Manage
{
    public class ShopInfoRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "배송비 기준 금액을 숫자로만 입력해 주십시오.")]
        public int standardprice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "배송비 금액을 숫자로만 입력해 주십시오.")]
        public int deliveryprice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "반품 배송비 금액을 숫자로만 입력해 주십시오.")]
        public int returndeliveryprice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "교환 배송비 금액을 숫자로만 입력해 주십시오.")]
        public int changedeliveryprice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "해외 배송비 기준 금액을 숫자로만 입력해 주십시오.")]
        public int foreignstandardprice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "해외 배송비 금액을 숫자로만 입력해 주십시오.")]
        public int foreigndeliveryprice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "해외 반품 배송비 금액을 숫자로만 입력해 주십시오.")]
        public int foreignreturndeliveryprice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "해외 교환 배송비 금액을 숫자로만 입력해 주십시오.")]
        public int foreignchangedeliveryprice { get; set; }

        [Required(ErrorMessage = "반송지 우편번호를 입력해 주십시오.")]
        public string rzipcode { get; set; }

        [Required(ErrorMessage = "반송지 주소를 입력해 주십시오.")]
        public string raddr1 { get; set; }

        [Required(ErrorMessage = "반송지 상세 주소를 입력해 주십시오.")]
        public string raddr2 { get; set; }

        [Required(ErrorMessage = "해외 반송지 우편번호를 입력해 주십시오.")]
        public string foreignrzipcode { get; set; }

        [Required(ErrorMessage = "해외 반송지 주소를 입력해 주십시오.")]
        public string foreignraddr1 { get; set; }

        [Required(ErrorMessage = "해외 반송지 상세 주소를 입력해 주십시오.")]
        public string foreignraddr2 { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "일반상품평 기준 포인트를 숫자로만 입력해 주십시오.")]
        public int txtreviewpoint { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "포토상품평 기준 포인트를 숫자로만 입력해 주십시오.")]
        public int imgreviewpoint { get; set; }
    }
}