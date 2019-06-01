import { getAsync, postAsync, putAsync, deleteAsync } from '../../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/transaction";

export async function getAllTransacBudgetAsync(){
  return await getAsync(`${endpoint}/getAllTransacBudget`);
}

export async function  getTransacBudgetAsync(transacBudgetId){
  return await getAsync(`${endpoint}/GetTransacBudget/${transacBudgetId}`);
}

export async function createTransacBugetAsync(model){
  return await postAsync(`${endpoint}/createTransacBudget`, model);
}

export async function updateTransacBudgetAsync(transacBudgetId){
  return await putAsync(`${endpoint}/updateTransacBudget/${transacBudgetId}`);
}

export async function deleteTransacBudgetAsync(transacBudgetId){
  return await deleteAsync(`${endpoint}/deleteTransacBudget/${transacBudgetId}`)
}

export async function getAllTransacDepenseAsync(){
  return await getAsync(`${endpoint}/getAllTransacDepense`);
}

export async function getTransacDepenseAsync(transacDepenseId){
  return await getAsync(`${endpoint}/gettransacadepense/${transacDepenseId}`);
}

export async function createTransacDepenseAsync(model){
  return await postAsync(`${endpoint}/createTransacDepense`, model);
}

export async function updateTransacDepenseAsync(transacDepenseId){
  return await putAsync(`${endpoint}/updateTransacDepense/${transacDepenseId}`);
}

export async function deleteTransacDepenseAsync(transacDepenseId){
  return await deleteAsync(`${endpoint}/deleteTransacDepense/${transacDepenseId}`)
}

export async function createTransactionAsync(model){
  return await postAsync(`${endpoint}/createTransaction`, model)
}