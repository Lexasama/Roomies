import { getAsync, postAsync, putAsync, deleteAsync } from '../../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/budget";

export async function getBudgetAsync(budgetId) {
  return await getAsync(`${endpoint}/getBudgetById/${budgetId}`);
}

export async function getAllBudgetAsync(collocId) {
  return await getAsync(`${endpoint}/getBudgets/${collocId}`);
}
export async function getAllBudgetOfCategoryAsync(categoryId) {
  return await getAsync(`${endpoint}/getBudgetsOfCategory/${categoryId}`);
}

export async function createBudgetAsync(model) {
  return await postAsync(`${endpoint}/add`, model);
}

export async function updateBudget(budgetId) {
  return await putAsync(`${endpoint}/update/${budgetId}`);

}

export async function getCurentBudgetOfCategory(categoryId){
  return await getAsync(`${endpoint}/currentBudget/${categoryId}`);
}

export async function deleteBudgetAsync(budgetId) {
  return await deleteAsync(`${endpoint}/delete/${budgetId}`);
}

export async function getAllBudgetCatAsync(collocId) {
  return await getAsync(`${endpoint}/getAllBudgetCatData/${collocId}`);
}

export async function getBudgetCatByTimeAsync(collocId, date) {
  return await getAsync(`${endpoint}/getBudgetByTime/${collocId}/${date}`);
}

export async function getDailyBudgetCatAsync(collocId) {
  return await getAsync(`${endpoint}/getDailyBudget/${collocId}`);
}

export async function getCategoryOffDates(categoryId) {
  return await getAsync(`${endpoint}/offDates/${categoryId}`);
}