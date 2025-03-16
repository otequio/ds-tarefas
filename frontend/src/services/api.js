import axios from 'axios';

const BASE_URL = import.meta.env.VITE_API_URL ?? "http://localhost:5240/api";
const API_URL = `${BASE_URL}/tarefas`

const obterTarefas = async (status) => {
    const resposta = await axios.get(`${API_URL}?status=${status}`);
    return resposta.data;
};

const obterTarefaPorId = async (id) => {
  const resposta = await axios.get(`${API_URL}/${id}`);
  return resposta.data;
};

const criarTarefa = async (tarefa) => {
    const resposta = await axios.post(API_URL, tarefa);
    return resposta.data;
};

const atualizarTarefa = async (tarefa) => {
    await axios.put(`${API_URL}/${tarefa.id}`, tarefa);
};

const excluirTarefa = async (id) => {
    await axios.delete(`${API_URL}/${id}`);
};

export default {
    obterTarefas,
    obterTarefaPorId,
    criarTarefa,
    atualizarTarefa,
    excluirTarefa
};