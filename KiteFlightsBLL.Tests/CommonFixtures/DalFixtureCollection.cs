﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KiteFlightsBLL.Tests.CommonFixtures
{
	[CollectionDefinition("DalFixtureCollection")]
	public class DalFixtureCollection : ICollectionFixture<DalFixture>
	{
	}
}