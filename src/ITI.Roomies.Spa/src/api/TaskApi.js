import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = process.env.VUE_APP_BACKEND + "/api/Task";

export async function getTasksByCollocIdAsync(collocId) {
    return await getAsync(`${endpoint}/getByCollocId/${collocId}`);
}

export async function GetTasksByRoomieIdAsync() {
    return await getAsync(`${endpoint}/getByRoomieId`);
}

export async function GetTaskByTaskIdAsync(taskId) {
    return await getAsync(`${endpoint}/getByTaskId/${taskId}`);
}

export async function createTaskAsync(model) {
    return await postAsync(`${endpoint}/createTask`, model);
}

export async function UpdateTaskStateAsync(taskId, taskState) {
    return await postAsync(`${endpoint}/updateTaskState/${taskId}/${taskState}`);
}

export async function UpdateTaskAsync(taskId, model) {
    return await postAsync(`${endpoint}/updateTask/${taskId}`, model);
}

export async function DeleteTaskByIdAsync(taskId) {
    return await deleteAsync(`${endpoint}/deleteTask/${taskId}`);
}