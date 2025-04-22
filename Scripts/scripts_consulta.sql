Use KarmaDB;

select us.Email, rol.Name 
from AspNetUserRoles ur, AspNetUsers us, AspNetRoles rol
where ur.RoleId = rol.Id and ur.UserId = us.Id

