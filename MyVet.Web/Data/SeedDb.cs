﻿using MyVet.Web.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckPetTypesAsync();
            await CheckServiceTypesAsync();
            await CheckOwnersAsync();
            await CheckPetsAsync();
            await CheckAgendasAsync();
        }

        private async Task CheckPetsAsync()
        {
            var owner = _context.Owners.FirstOrDefault();
            var petType = _context.PetTypes.FirstOrDefault();
            if (!_context.Pets.Any())
            {
                AddPet("Otto", owner, petType, "Shih tzu");
                AddPet("Killer", owner, petType, "Dobermann");
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckServiceTypesAsync()
        {
            if (!_context.ServiceTypes.Any())
            {
                _context.ServiceTypes.Add(new ServiceType { Name = "Consulta" });
                _context.ServiceTypes.Add(new ServiceType { Name = "Urgencia" });
                _context.ServiceTypes.Add(new ServiceType { Name = "Vacunación" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPetTypesAsync()
        {
            if (!_context.PetTypes.Any())
            {
                _context.PetTypes.Add(new PetType { Name = "Perro" });
                _context.PetTypes.Add(new PetType { Name = "Geto" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckOwnersAsync()
        {
            if (!_context.Owners.Any())
            {
                AddOwner("123456", "Kevin", "Hernández", "234 3232", "301 221 85 27", "Cll 94$ 34 54");
                AddOwner("789121", "Brenda", "Echeverri", "343 3226", "301 221 85 27", "Cll 94$ 34 54");
                AddOwner("454312", "Yanit", "López", "450 4332", "301 221 85 27", "Cll 94$ 34 54");
                await _context.SaveChangesAsync();
            } 
        }

        private void AddOwner(string document, string firstName, string lastName, string fixedPhone, string cellPhone, string address)
        {
            _context.Owners.Add(new Owner
            {
                Address = address,
                CellPhone = cellPhone,
                Document = document,
                FirstName = firstName,
                FixedPhone = fixedPhone,
                LastName = lastName
            });
        }

        private void AddPet(string name, Owner owner, PetType petType, string race)
        {
            _context.Pets.Add(new Pet
            {
                Born = DateTime.Now.AddYears(-2),
                Name = name,
                Owner = owner,
                PetType = petType,
                Race = race
            });
        }

        private async Task CheckAgendasAsync()
        {
            if (!_context.Agendas.Any())
            {
                var initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
                var finalDate = initialDate.AddYears(1);
                while (initialDate < finalDate)
                {
                    if (initialDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var finalDate2 = initialDate.AddHours(10);
                        while (initialDate < finalDate2)
                        {
                            _context.Agendas.Add(new Agenda
                            {
                                Date = initialDate.ToUniversalTime(),
                                IsAvailable = true
                            });

                            initialDate = initialDate.AddMinutes(30);
                        }

                        initialDate = initialDate.AddHours(14);
                    }
                    else
                    {
                        initialDate = initialDate.AddDays(1);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
