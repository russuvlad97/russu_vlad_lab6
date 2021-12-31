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

        public override Task<CustomerList> GetAll(Empty empty, ServerCallContext context)
        {

            CustomerList pl = new CustomerList();
            var query = from cust in db.Customers
                        select new Customer()
                        {
                            CustomerId = cust.CustomerID,
                            Name = cust.Name,
                            Adress = cust.Address
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
                Address = requestData.Adress,
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }
    }
}
