from ortools.sat.python import cp_model

def SeatingArrangement():
    # Creates the model
    model = cp_model.CpModel()

    num_guests = 100
    num_tables = 10
    table_capacity = 10

    # x[i, j] is True if guest i is at table j.
    x = {}
    for i in range(num_guests):
        for j in range(num_tables):
            x[(i, j)] = model.NewBoolVar(f'x[{i},{j}]')

    # secili musafir ulet saktesisht ne nje tavoline
    for i in range(num_guests):
        model.Add(sum(x[i, j] for j in range(num_tables)) == 1)

    must_sit = [(0, 1), (5, 6)]  # shuembull, 0 me 1 ulen ne nje tavoline dhe 5 me 6 ulen ne nje tavoline
    cannot_sit = [(2, 3), (7, 8)] # shembull 2 me 3 nuk munden mu ul bashk dhe 7 me 8 nuk munden


    for a, b in must_sit:
        for j in range(num_tables):
            model.Add(x[a, j] == x[b, j])

    for a, b in cannot_sit:
        for j in range(num_tables):
            model.Add(x[a, j] + x[b, j] <= 1)

    # secila tavoline ka 10 musafir
    for j in range(num_tables):
        model.Add(sum(x[i, j] for i in range(num_guests)) == table_capacity)

    # Creates a solver and solves the model.
    solver = cp_model.CpSolver()
    status = solver.Solve(model)

    if status == cp_model.FEASIBLE or status == cp_model.OPTIMAL:
        for j in range(num_tables):
            print(f'Table {j}: ', end='')
            for i in range(num_guests):
                if solver.Value(x[(i, j)]) == 1:
                    print(f'Guest {i} ', end='')
            print()
    else:
        print("No solution found.")

SeatingArrangement()
