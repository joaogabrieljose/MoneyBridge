using CustomerService.Application.DTOs.Requests;
using CustomerService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCustomerRequest request)
    {
        var result = await _customerService.CreateAsync(request);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Customer.EmailAlreadyExists" ||
                result.Error.Code == "Customer.TaxNumberAlreadyExists")
            {
                return Conflict(new { code = result.Error.Code, message = result.Error.Message });
            }

            return BadRequest(new { code = result.Error.Code, message = result.Error.Message });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

  
    

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _customerService.GetAllAsync();

        if (result.IsFailure)
        {
            return BadRequest(new { code = result.Error.Code, message = result.Error.Message });
        }

        return Ok(result.Value);
    }



    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdAsync(id);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Customer.NotFound")
            {
                return NotFound(new { code = result.Error.Code, message = result.Error.Message });
            }

            return BadRequest(new { code = result.Error.Code, message = result.Error.Message });
        }

        return Ok(result.Value);
    }
    

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerRequest request)
    {
        var result = await _customerService.UpdateAsync(id, request);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Customer.NotFound")
            {
                return NotFound(new { code = result.Error.Code, message = result.Error.Message });
            }

            if (result.Error.Code == "Customer.EmailAlreadyExists" ||
                result.Error.Code == "Customer.TaxNumberAlreadyExists")
            {
                return Conflict(new { code = result.Error.Code, message = result.Error.Message });
            }

            return BadRequest(new { code = result.Error.Code, message = result.Error.Message });
        }

        return Ok(result.Value);
    }
    
    

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _customerService.DeleteAsync(id);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Customer.NotFound")
            {
                return NotFound(new { code = result.Error.Code, message = result.Error.Message });
            }
            
            return BadRequest(new { code = result.Error.Code, message = result.Error.Message });
        }

        return NoContent();
    }


    

    [HttpGet("page")]
    public async Task<IActionResult> GetPage(int pageNumber, int pageQuantity)
    {
        var result =  await _customerService.GetPaginatedResultAsync(pageNumber, pageQuantity);

        if (result.IsFailure)
        {
            return BadRequest(new { code = result.Error.Code, message = result.Error.Message });
        }

        return Ok(result.Value);
    }
}