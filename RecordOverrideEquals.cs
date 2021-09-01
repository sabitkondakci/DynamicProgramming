void Main()
{
	var biden1 = new Student("Joe","Biden");
	var biden2 = new Student("Joe","biden");
	
	biden1.Equals(biden2).Dump();
}

record Student (string FirstName, string LastName)
{
  // this'll be consumed at runtime
	public virtual bool Equals(Student student)
	{
		if (student is null)
			return false;

		// If run-time types are not exactly the same, return false.
		if (this.EqualityContract != student.EqualityContract)
			return false;

		// Optimization for a common success case.
		if (object.ReferenceEquals(this, student))
			return true;
		
		return (FirstName.ToLower() == student.FirstName.ToLower())
		&& (LastName.ToLower() == student.LastName.ToLower());
	}
	
	public override int GetHashCode() => (FirstName,LastName).GetHashCode();
}
