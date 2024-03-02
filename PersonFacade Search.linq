<Query Kind="Statements" />

string first = "";
string middle = "";
string last = "Johnson";

var date = DateTime.Now;
var fnlc = !string.IsNullOrWhiteSpace(first) ? first.ToLower() : "";
var mnlc = !string.IsNullOrWhiteSpace(middle) ? middle.ToLower() : "";
var lnlc = !string.IsNullOrWhiteSpace(last) ? last.ToLower() : "";

var omittedRelationshipTypes = new[] { (int)RelationshipTypeEnum.BirthFather, (int)RelationshipTypeEnum.BirthMother };
var typeArray = new[] {
						(int) PersonTypeEnum.Student,
						(int) PersonTypeEnum.ParentGuardian,
						(int) PersonTypeEnum.Staff,
						(int) PersonTypeEnum.ParentPortal
					};
					
var search = People.SearchByName(first, middle, last, NameComparisonType.StartsWith).Where(w => w.PersonID != 550086 && w.PersonPersonTypes.Any() && w.PersonPersonTypes.Any(a => typeArray.Contains(a.PersonType.PersonTypeID))).Where(w => w.RelationshipsInverted.All(a => !omittedRelationshipTypes.Contains(a.RelationshipType.RelationshipTypeID)));

search = from p in search
		 let flc = p.FirstName.ToLower()
		 let mlc = p.MiddleName == null ? "" : p.MiddleName.ToLower()
		 let llc = p.LastName.ToLower()
		 let fsw = flc.StartsWith(fnlc)
		 let fc = flc.Contains(fnlc)
		 let msw = mlc.StartsWith(mnlc)
		 let mc = mlc.Contains(mnlc)
		 let lsw = llc.StartsWith(lnlc)
		 let lc = llc.Contains(lnlc)
		 orderby lsw && fsw && msw // All names match start
			 ? 0
			 : lsw && fsw // First and last match start
				 ? 5
				 : lsw || fsw || msw // Any names match start
					 ? 10
					 : lc && fc && mc // All names contain
						 ? 20
						 : lc && fc // First and last contain
							 ? 25
							 : lc || fc || mc // Any names contain
								 ? 30
								 : 40, // None contain
			 p.FirstName,
			 p.MiddleName,
			 p.LastName
		 select p;
var addresses = new List<string>();
//var types = new List<PersonTypeDC>();
var matches = search.Take(6)
								.Select(p => new ContactSearchResultDC
								{
									PersonID = p.PersonID,
									FirstName = p.FirstName,
									MiddleName = p.MiddleName,
									LastName = p.LastName,
									DateOfBirth = p.DateOfBirth,

																		Addresses = (
																		    from pl in PersonLocations
																		    where pl.Person.PersonID == p.PersonID
																		    && pl.EffectiveStartDate <= date && (date <= pl.EffectiveEndDate || pl.EffectiveEndDate == null)
																		    select pl.Address.AddressLine1
																		).Distinct().ToList(),
									//Occupation = p.ParentGuardians.Select(pg => pg.Occupation).FirstOrDefault() ?? p.Staffs.Select(s => s.StaffDepartments.Select(d => d.Department.DepartmentName).FirstOrDefault()).FirstOrDefault(),
									//Types = p.PersonPersonTypes.Select(a => new PersonTypeDC
									//									{
									//										Description = a.PersonType.Description,
									//										PersonTypeID = a.PersonType.PersonTypeID
									//									}).ToList()
									//Types = types.Select(t => t).ToList()//.AsEnumerable().Select(s => new PersonTypeDC { Description = "", PersonTypeID = 0}).ToList()
								})
								.ToList();

matches.Dump();