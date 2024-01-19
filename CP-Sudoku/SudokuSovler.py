import time
import constraint

def solve(s):
    start_time = time.time() 

    problem = constraint.Problem()

    for r in range(9):
        for c in range(9):
            n = int(s[c+10*r])
            problem.addVariable('n_{}_{}'.format(r,c), [n] if n>0 else range(1, 10))

    for r in range(9):
        for c in range(9):
            nrc='n_{}_{}'.format(r,c)
            for i in range(9):
                if i>r:
                    problem.addConstraint(lambda x, y: x!=y, (nrc, 'n_{}_{}'.format(i,c)))
                if i>c:
                    problem.addConstraint(lambda x, y: x!=y, (nrc, 'n_{}_{}'.format(r,i)))
    
    for r0 in range(0,9,3):
        for c0 in range(0,9,3):
            for r in range(r0, r0+3):
                for c in range(c0, c0+3):
                    nrc = 'n_{}_{}'.format(r, c)
                    for r2 in range(r0, r0+3):
                        for c2 in range(c0, c0 + 3):
                            if r2>r or  (r2 == r and c2 > c):
                                problem.addConstraint(lambda x, y: x!=y, (nrc, 'n_{}_{}'.format(r2, c2)))
                    
    solve_start_time = time.time()   
    solutions = problem.getSolutions()
    solve_end_time = time.time() 

    total_time = solve_end_time - start_time 

    for solution in solutions:
        print('--')
        for r in range(9):
            for c in range(9):
                print('{}\t'.format(solution['n_{}_{}'.format(r,c)]), end='')
            print()

    print(f"Total time: {total_time:.6f} seconds")
    print(f"Solving time: {solve_end_time - solve_start_time:.6f} seconds")

s='000043900;005000000;004700601;020070003;340050069;600010050;102008700;000000800;008930000'
solve(s)