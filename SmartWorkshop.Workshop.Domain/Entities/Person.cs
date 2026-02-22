using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.ValueObjects;

namespace SmartWorkshop.Workshop.Domain.Entities;

public class Person : Entity
{
    private Person() { }

    public Person(string fullname, string document, PersonType personType, EmployeeRole? employeeRole, string email, string password, Phone phone, Address? address)
    {
        Address = new Address(string.Empty, string.Empty, string.Empty, string.Empty);
        Update(fullname, document, personType, employeeRole, email, password, phone, address);
    }

    public Document Document { get; private set; } = string.Empty;
    public string Fullname { get; private set; } = string.Empty;
    public PersonType PersonType { get; private set; }
    public EmployeeRole? EmployeeRole { get; private set; }
    public Phone Phone { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public Guid AddressId { get; private set; }
    public Address Address { get; private set; } = null!;
    public ICollection<Vehicle> Vehicles { get; private set; } = [];

    public Person Update(string fullname, string document, PersonType personType, EmployeeRole? employeeRole, string email, string password, Phone phone, Address? address)
    {
        if (!string.IsNullOrEmpty(document)) Document = document;
        if (!string.IsNullOrEmpty(fullname)) Fullname = fullname;
        if (!string.IsNullOrEmpty(password)) Password = password;
        PersonType = personType;
        EmployeeRole = employeeRole;
        UpdatePhone(phone);
        UpdateEmail(email);
        UpdateAddress(address);
        return this;
    }

    private void UpdatePhone(Phone? phone)
    {
        if (phone is null) return;
        Phone = new Phone(phone.AreaCode, phone.Number);
    }

    private void UpdateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return;
        Email = email;
    }

    private void UpdateAddress(Address? address) => Address.Update(address);

    public void Validate()
    {
        if (PersonType == PersonType.Client && EmployeeRole != null)
        {
            throw new DomainException("Client cannot have an employee role.");
        }

        if (!Document.IsValid())
        {
            throw new DomainException("Document is not valid.");
        }

        if (Email != null && !Email.IsValid())
        {
            throw new DomainException("Email is not valid.");
        }
    }
}
