using Microsoft.EntityFrameworkCore;
using OpenERP.Data;
using OpenERP.Enums.Global;
using OpenERP.Enums.HumanResource;
using OpenERP.Models.Global;
using OpenERP.ViewModels.Global.Contacts;

namespace OpenERP.Services.Global
{
    public class ContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpsertContacts(
            string modelType,
            int modelId,
            List<ContactViewModel> contactModels)
        {
            foreach (var contactRequest in contactModels)
            {
                if (!Enum.TryParse(contactRequest.Type, true, out ContactType contactType))
                    throw new ArgumentException("Invalid contact type");

                ContactRelationType? contactRelationType = null;
                if (contactRequest.ContactRelationType != null)
                {
                    if (!Enum.TryParse(contactRequest.ContactRelationType, true, out ContactRelationType parsedContactRelationType))
                        throw new ArgumentException("Invalid contact relation type");

                    contactRelationType = parsedContactRelationType;
                }

                var existingContact = await _context.Contacts
                    .Where(c => c.ModelType == modelType && c.ModelId == modelId && c.Id == contactRequest.Id)
                    .FirstOrDefaultAsync();

                if (existingContact == null)
                {
                    _context.Contacts.Add(new Contact
                    {
                        ModelType = modelType,
                        ModelId = modelId,
                        Type = contactType,
                        Information = contactRequest.Information,
                        ContactName = contactRequest.ContactName,
                        ContactRelationType = contactRelationType
                    });

                    await _context.SaveChangesAsync();
                }
                else
                {
                    existingContact.Type = contactType;
                    existingContact.Information = contactRequest.Information;
                    existingContact.ContactName = contactRequest.ContactName;
                    existingContact.ContactRelationType = contactRelationType;

                    _context.Contacts.Update(existingContact);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveContacts(
            string modelType,
            int modelId,
            List<ContactViewModel> contactModels)
        {
            var existingContactIds = contactModels.Select(c => c.Id).ToList();
            var contactsToRemove = await _context.Contacts
                .Where(c => c.ModelType == modelType && c.ModelId == modelId && !existingContactIds.Contains(c.Id))
                .ToListAsync();

            _context.Contacts.RemoveRange(contactsToRemove);

            await _context.SaveChangesAsync();
        }
    }
}