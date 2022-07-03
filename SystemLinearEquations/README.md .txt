
This project contains 3 different algorithms for solving systems of linear equations
		
		Ax = b	

		where A is a (n x n) matrix with real values
		and x, b are (n) vectors with real values


- Exact Methods

	Cramer's rule (published in 1750)
	
		The solution vector x = <x1,...,xn> for arbitrary dimensions n
		can be found by doing the following operation

		xi = det(Ai(b)) / det(A) for i = 1,..,n,
		
		where Ai(b) is the original matrix A with a swapped column vector b in the ith column
		
		Therefore this method involves calculating the determinant of n + 1 matrices

		O(n * determinant)


	Laplacian expansion (Laplace lived 1749 – 1827)

		det(A) = Sum(i, j){ (-1)^i * A[i,j] * det(Mij) } for i = 1,...,n  j = 1,...,n

		where A[i,j] is the value of the element at (i,j)

		and	Mij is the minor matrix of A at (i,j)

		therefore this function recursively calls itself

		O(n!)

	
	Thus the overall complexity of Cramer's rule using Laplacian expansion is

		O(n!)


	These methods are used together in proofs, but are impractical for computing large matricies 
	(on my computer it started taking minutes at around n = 10, and started using too much memory to even continue at around n = 12)

	Note: Even the wikipedia article for Cramer's rule doesn't specify that the method used for 
	computing the determinant is what is slowing this implementation down



- Approximate methods
	
	The conjugate gradient (1952) and biconjugate gradient (1992) methods are approximate solutions

	I won't give as detailed an explanation of these methods because I am not implementing them in any novel way 
	For more info go to their wikipedia pages

	Basically the gradient methods use an input guess 
	
	x = <x1,...,xn> 
	
	(2 guesses in the biconjugate case)

	and then try to iteratively produce better guesses until the desired error is achieved 
	(aka when the residual vector is sufficiently small) r = b - Ax ~ 0
	
	The big difference between these two methods is that the conjugate method can only solve hermitian matricies
	(symmetric across the diagonal) while the biconjugate method can solve any real matrix. Though in practice you should not
	solve hermitian systems with the biconjugate method as you would be doing twice the computational effort with no payoff
	
	
#################################################################################################################################


										END OF MATH SECTION, CODE DESCRIPTION BELOW



#################################################################################################################################

Code description:

- a modified version of Cramer's rule with Laplacian expansion O(n!) => O(n) & Aux(2^n)

	-	1 persistent Dictionary<string,double> with Aux(2^n), the minors   first determinant calculated, det(A)

	- 	and n * Dictionary<string,double> with Aux(2^n-1) which are not stored in memory
	
	-	where the key is a coordinate of the minor matrix being calculated for that
		recursive call to getMinorDeterminant()

	- 	addtionally the key is encoded using ints(representing matrix columns) converted to bytes and then to a string
		
		-	with a limit on n = 2^8 = 256

			(the dictionary size will make reaching this limit impractical,
			but it remains a good limit because it means every column can be represented by a byte, 
			so no need to have a character like ',' seperating numbers, which further cuts down on key size)

		-	the size of the key is dependent on the size of byte[].Length

			pseudo code

			byte[] b = { "1", "2", "5", "7", "9" } is 5 bytes converted to a string, 

			instead of for example

			string s = "1,2,5,7,9" a string of 10 chars
			


	- on my machine I went from calculating n=11 in ~2 minutes (with the naive approach) to n=21 in ~2 minutes, 
		though this solution slows down at around n=23



- a standard implementation of the Conjugate Gradient method

	- on my machine this solves a system where n = 1000 in ~ 35 seconds

- a standard implementation of the Biconjugate Gradient method

	- on my machine this solves a system where n = 300 in ~ 14 seconds

- a console app that exercises these methods and compares their runttime and outputs

	- When comparing the exact solution with an approximation, I use the exact solution as a reference and calculate the percent diff in length

	- Since both Conjugate methods sometimes produces unstable solutions (with very large difference in length), 
		I sometimes have to rerun a comparison with a new matrix (Need to study these errors,)