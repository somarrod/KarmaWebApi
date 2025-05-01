using System.ComponentModel.DataAnnotations;

public class RoleValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            string role = value.ToString();
            if (role == "AG_Admin" || role == "AG_Alumne" || role == "AG_Professor")
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult("El campo Role debe ser AG_Admin, AG_Alumne o AG_Professor.");
    }
}
