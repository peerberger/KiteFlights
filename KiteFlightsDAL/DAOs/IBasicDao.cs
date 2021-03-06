﻿using KiteFlightsDAL.POCOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiteFlightsDAL.DAOs
{
	public interface IBasicDao<TEntity> where TEntity : IPoco
	{
		// getting
		TEntity GetById(int id);
		IList<TEntity> GetAll();

		// adding
		int Add(TEntity entity);

		// updating
		bool Update(TEntity entity);

		// removing
		bool Remove(TEntity entity);
	}
}
