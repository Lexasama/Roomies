import { getAsync, postAsync, putAsync, deleteAsync } from '../../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/budget";

export async function getBudgetAsync (budgetId) {
  return await getAsync(`${endpoint}/getBudgetById/${budgetId}`);
}

export async function getAllBudgetAsync(collocId) {
  return await getAsync(`${endpoint}/getBudgets/${collocId}`);
}

export async function createBudgetAsync(model) {
  return await postAsync(`${endpoint}/add`, model);
}

export async function updateBudget(budgetId) {
  return await putAsync(`${endpoint}/update/${budgetId}`);

}

export async function deleteBudgetAsync(budgetId) {
  return await deleteAsync(`${endpoint}/delete/${budgetId}`);
}