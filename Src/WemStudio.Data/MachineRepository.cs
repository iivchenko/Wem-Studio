using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace WemManagementStudio.Data
{
    public sealed class MachineRepository : IRepository<Machine, long>
    {
        private readonly VMContext _context;

        public MachineRepository(VMContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }

        public void Add(Machine entity)
        {
            _context.Machines.Add(entity);
            _context.SaveChanges();
        }

        public void Remove(Machine entity)
        {
            _context.Machines.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Machine entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Machine Get(long key)
        {
            return
                _context
                    .Machines
                    .Find(key);
        }

        public IEnumerable<Machine> FindAll()
        {
            return
                _context
                    .Machines
                    .AsEnumerable();
        }

        public IEnumerable<Machine> Find(Expression<Func<Machine, bool>> query)
        {
            return
                _context
                    .Machines
                    .Where(query)
                    .AsEnumerable();
        }
    }
}
