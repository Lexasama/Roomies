import { getAsync, postAsync, putAsync, deleteAsync } from '../../../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/tbudget";

export async function getAllTransacBudgetAsync(){
  return await getAsync(`${endpoint}/getAllTBudget`);
}

export async function  getTransacBudgetAsync(transacBudgetId){
  return await getAsync(`${endpoint}/getTBudget/${transacBudgetId}`);
}

export async function createTransacBugetAsync(model){
  return await postAsync(`${endpoint}/createTBudget`, model);
}

export async function updateTransacBudgetAsync(model){
  return await putAsync(`${endpoint}/updateTBudget/${model.TBudgetId}`, model);
}
export async function deleteTransacBudgetAsync(transacBudgetId){
  return await deleteAsync(`${endpoint}/deleteTBudget/${transacBudgetId}`)
}

export async function getAllExpenses(budgetId){
  return await getAsync(`${endpoint}/getAllDepense/${budgetId}`);
}