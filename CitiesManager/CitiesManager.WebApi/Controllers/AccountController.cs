using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager,IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid == false)
            {
                string errorMessages = string.Join("|", ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage));
                return Problem(errorMessages);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName
            };
            IdentityResult result = await _userManager.CreateAsync(user,registerDTO.Password);
            if(result.Succeeded)
            {
                //Automatic sign in
                await _signInManager.SignInAsync(user, isPersistent: false);
                AuthenticationResponse authenticationResponse=  _jwtService.CreateJwtToken(user);
                return Ok(authenticationResponse);
            }
            else
            {
                string errorMessages = string.Join ("|",result.Errors.Select(e => e.Description)); //error1 | error2
                return Problem(errorMessages);
            }

        }

        [HttpGet]
        public async Task<IActionResult> IsEmailAlreadyRegister(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Ok(true);// user is not registered
            }
            else
            {
                return Ok(false); // user is already registerd
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostLogin(LoginDTO loginDTO)
        {
            //validation
            if (ModelState.IsValid== false)
            {
                string errorMessage = string.Join(" | ",ModelState.Values.SelectMany(x => x.Errors).Select(e=>e.ErrorMessage));
                return Problem(errorMessage);
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false,lockoutOnFailure:false);
            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if(user == null)
                {
                    return NoContent();
                }
                // sign in
                await _signInManager.SignInAsync(user, isPersistent: false);
                AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
                return Ok(authenticationResponse);
            }
            else
            {
                return Problem("Invalid email and password");
            }
            
        }

        [HttpGet("logout")]
        public async Task<IActionResult> GetLogout()
        {
            await _signInManager.SignOutAsync();    
            return Ok();
        }

    }
}
