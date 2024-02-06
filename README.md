
Done:
1. Make class lib of all vehicles. Add property for toll-free. 
2. Fix vehicles list, and toll free vehiclees. Bus was missing. Motorbike mispelled?
3. Separate logic from values with repositories. Better to have that in a potential database (easier to implement when we have repositories structure).

4. fix Logic:
    - Sort incoming parameter of dates (potential bug if we always take first - its not neccessary the firts timestamp.)
	- Fix day separator. Cause consumer can add multiple days. Days wheren't in calculation at all. Comment said to calculate for one day. Assignment did not? 
	- time intervals had one minute on each hour lost. 
	- other bugs discovered when tested with unit test.		

5. Unit testing
			
6. Create API solution with minor validation.


If more time/todo:
Entities/Models structure. Repositories should only use entities. Core-lib should have Models which entities are mapped to.
Maybe fix the Vehicle-type thing.
Exceptionhandling and logging. Out of scope exceptions in repositories for example.
Request validation and typing of input params can be better for usability, also add swagger comments.
Structure Service-class a bit.
More unit-test
- At last time i noticed that july would be tax-free month. That data should be stored in repository also, ( as a "TaxFreeMonths"-property.)