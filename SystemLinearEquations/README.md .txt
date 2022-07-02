This project contains 3 different algorithms for solving systems of linear equations
(in the next commit I will introduce the naive approach [Cramer's rule with recursive laplacian expansion])

Currently I present:
- a modified version of Cramer's rule O(n!) => O(n) & Aux(2^n)

	+	1 persistent Dictionary<string,double>(2^n)

	+ 	and n * Dictionary<string,double>(2^n-1) which are not stored in memory [GC]
	
	+	where the key is a coordinate of the minor matrix being calculated for that
		recursive call to getMinorDeterminant()

	+ 	addtionally the key is encoded using ints(representing matrix columns) converted to bytes and then to a string
		-	with an arbitrary limit on column length of 2^8
			(arbitraty because the dictionary size will make this limit irrelevant)
			(but it remains a good limit because it can be represented in a byte)

			the size of the key is dependent on the size of byte[].Length
			"[1][2][5][7][9]" is 5 bytes converted to a string, rather than 15 char converted to a string

- a standard implementation of the Conjugate Gradient method (
	+ 	not recursive but iterative, with an arbitrary limit on the amount of iterations

- a standard implementation of the Biconjugate Gradient method
	+	" "


To do:
- Add a naive implentation of Cramer's Rule to show performance improvement of using dictionaries

- Add a better heuristic for selection of initial guesses for Conjugate methods

- Figure out complexity of conjugate gradient implementations