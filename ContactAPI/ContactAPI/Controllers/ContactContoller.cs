using ContactAPI.Data;
using ContactAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactContoller : Controller
    {
        public readonly ContactAPIDBContext dbContext;
        public static IWebHostEnvironment _environment;


        public ContactContoller(ContactAPIDBContext dbContext, IWebHostEnvironment environment)
        {
            this.dbContext = dbContext;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromForm] AddContactRequest addContactRequest)
        {
            try
            {
                if (addContactRequest.files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Uploads\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Uploads\\" + addContactRequest.Email + "_" + DateTimeOffset.Now.ToUnixTimeMilliseconds()+ "_" + addContactRequest.files.FileName))
                    {
                        addContactRequest.files.CopyTo(fileStream);
                        fileStream.Flush();
                    }

                }
                else
                {
                    Console.WriteLine("erro na imagem");
                }

                var contact = new Contact()
                {
                    Id = Guid.NewGuid(),
                    Address = addContactRequest.Address,
                    Email = addContactRequest.Email,
                    FullName = addContactRequest.FullName,
                    Phone = addContactRequest.Phone,
                    files= addContactRequest.files,
                    anexoName = addContactRequest.anexoName,
                };

                await dbContext.Contacts.AddAsync(contact);
                await dbContext.SaveChangesAsync();

                return Ok(contact);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact != null)
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Phone = updateContactRequest.Phone;
                contact.Address = updateContactRequest.Address;
                contact.Email = updateContactRequest.Email;

                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }

            return NotFound();

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if (contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }

            return NotFound();
        }
    }
}
