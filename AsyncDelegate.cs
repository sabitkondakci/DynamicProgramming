

class Program
{
	
	
	public record User(string FirstName, string LastName, string Email, int? Age);
	
	// async delegates give us a good degree of freedom & flexibility
	// without corrupting the application
	
	public static async Task<User> GetUserInfoByEmail(string Email,
	Func<string, Task<User>> searchAlgorithm = null)
	{
		searchAlgorithm ??= DefaultEmailSearchingAlgorithm;	
		return await searchAlgorithm(Email);
	}
	
	public static async Task<IEnumerable<User>> GetUserListByFirstName( string FirstName,
	Func<string, Task<IEnumerable<User>>> searchAlgorithm = null)
	{
		searchAlgorithm ??=DefaultFirstNameSearchingAlgorithm;
		return await searchAlgorithm(FirstName);
	}

	public static async Task<User> DefaultEmailSearchingAlgorithm(string email)
	{
		if(email == null || email == string.Empty)
			return await Task.Run (() => new User(string.Empty,string.Empty,string.Empty,0));
			
		return await Task.Run(() => userList.First(l => l.Email == email));
	}

	public static async Task<IEnumerable<User>> CustomFirstNameSearchingAlgorithm(string firstName)
	{
		if (firstName == null || firstName == string.Empty)
			return await Task.Run (() => userList.DefaultIfEmpty());
			
		return await Task.Run(() => userList.Where(l => l.FirstName == firstName));
	}
	
	public static async Task<IEnumerable<User>> DefaultFirstNameSearchingAlgorithm(string firstName)
	{
		return await Task.Run(() => userList.Where(l => l.FirstName == firstName).OrderBy(l => l.Age));
	}

	static IEnumerable<User> userList = new List<User>()
	{
		new User("Halil","Sezai","sezaihalil@outlook.com",32),
		new User("Onur","Kocabiyil","onurkocabiyil@outlook.com",27),
		new User("Emin","Otlak","eminotlak@outlook.com",16),
		new User("Sehtap","Ikildi","sehtapikildi@gmail.com",43),
		new User("Sehtap","Adali","gamzeardali@gmail.com",36),
	};

	static async Task Main(string[] args)
	{
		User userByEmailDefault = await GetUserInfoByEmail("");
		IEnumerable<User> userByFirstNameDefault = await GetUserListByFirstName("Sehtap");

		User userByEmail = await GetUserInfoByEmail("eminotlak@outlook.com");
		IEnumerable<User> userByFirstNameCustom = await GetUserListByFirstName("Sehtap",
		CustomFirstNameSearchingAlgorithm);
		
		userByEmail.Dump();
		userByFirstNameCustom.Dump();
	}
}
