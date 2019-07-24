using GestorInventarioEmpresas.BackEnd.Commons.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GestorInventarioEmpresas.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
         public long id { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Debe ingresar un Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
        [MinLength(3, ErrorMessage = "El nombre de usuario debe tener 3 caracteres alfanuméricos como minimo")]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Debe ingresar un nombre completo")]
        [MinLength(3, ErrorMessage = "El nombre completo debe tener 3 caracteres alfanuméricos como minimo")]
        [Display(Name = "Nombre completo")]
        public string Name { get; set; }
        public TypeEmployerEnum TypeEmployer { get; set; }

        public LocationEnum Location { get; set; }
        //[Required]
        //[StringLength(100, ErrorMessage =
        // "Debe tener entre {0} y {2} caracteres de largo.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Contraseña de confirmación")]
        //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage =
        // "La contraseña y la contraseña de confirmación no coinciden")]
        //public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Rol de usuario")]
        public string UserRoles { get; set; }
        public SelectList UserRolesItems { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Debe tener entre {0} y {2} caracteres de largo.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña de confirmación")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage =  "La contraseña y la contraseña de confirmación no coinciden")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class Users_in_Role_ViewModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

}
