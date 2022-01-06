using Grpc.Core;
using GrpcCustomersService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess = LibraryModel.Data;
using ModelAccess = LibraryModel.Models;

namespace GrpcCustomerService
{
    public class GrpcCrudService : CustomerService.CustomerServiceBase
    {
        private DataAccess.LibraryContext db = null;

        public GrpcCrudService(DataAccess.LibraryContext db)
        {
            this.db = db;
        }

        public override Task<Customer> Get (CustomerId requestData, ServerCallContext context)
         {
            var data = db.Customers.Find(requestData.Id);

            Customer emp = new Customer()
            {
                CustomerId = data.CustomerID,
                Name = data.Name,
                Address = data.Address
            };

            return Task.FromResult(emp);
         }

        public override Task<CustomerList> GetAll(Empty empty, ServerCallContext context)
        {

            CustomerList pl = new CustomerList();
            var query = from cust in db.Customers
                        select new Customer()
                        {
                            CustomerId = cust.CustomerID,
                            Name = cust.Name,
                            Address = cust.Address
                        };
            pl.Item.AddRange(query.ToArray());
            return Task.FromResult(pl);
        }

        public override Task<Empty> Insert(Customer requestData, ServerCallContext context)
        {
            db.Customers.Add(new ModelAccess.Customer
            {
                CustomerID = requestData.CustomerId,
                Name = requestData.Name,
                Address = requestData.Address,
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> Delete(CustomerId requestData, ServerCallContext context)
        {
            var data = db.Customers.Find(requestData.Id);

            db.Customers.Remove(data);
            db.SaveChanges();

            return Task.FromResult(new Empty());
        }

        public override Task<Empty> Update(Customer requestData, ServerCallContext context)
        {
            db.Customers.Update(new ModelAccess.Customer()
                {
                CustomerID = requestData.CustomerId,
                Name = requestData.Name,
                Adress = requestData.Adress,
                BirthDate = DateTime.Parse(requestData.Birthdate)
                });
            db.SaveChanges();

            return Task.FromResult(new Empty());
        }
    }
}
