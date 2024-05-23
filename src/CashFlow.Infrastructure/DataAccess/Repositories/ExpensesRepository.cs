﻿namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository : IExpensesReadOnlyRepository, 
                                    IExpensesWriteOnlyRepository,
                                    IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task Add(Expense expense) =>
        await _dbContext.Expenses.AddAsync(expense);

    public async Task<List<Expense>> GetAll() =>
        await _dbContext.Expenses.AsNoTracking().ToListAsync();

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id) =>
        await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id) =>
        await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == id);

    public void Update(Expense expense) =>
        _dbContext.Expenses.Update(expense);

    public async Task<bool> Delete(long id)
    {
        var result = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
        
        if (result is null)
            return false;

        _dbContext.Expenses.Remove(result);

        return true;
    }
}
