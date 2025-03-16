import React, { useState, useEffect } from 'react';
import { Form, Button, Stack} from 'react-bootstrap';
import api from '../services/api';
import { Link } from 'react-router';
import { useParams, useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const FormularioTarefa = () => {
    const [tarefa, setTarefa] = useState({ titulo: '', descricao: '', status: 'Pendente', dataConclusao: '' });
    const { id } = useParams();
    const navigate = useNavigate()

    useEffect(() => {
        if (!id) 
            return;

        api.obterTarefaPorId(id)
          .then(result => setTarefa(result))
    }, [id]);

    useEffect(() => {
        setTarefa((prevTarefa) => {
            if (prevTarefa.status === 'Concluida') {
                return { ...prevTarefa, dataConclusao: new Date().toISOString() };
            } else {
                return { ...prevTarefa, dataConclusao: '' };
            }
        });
    }, [tarefa.status])

    const enviar = async (e) => {
        e.preventDefault();
        if (id) {
            await api.atualizarTarefa(tarefa)
                .then(navigate('/'))
                .catch(erro => {
                    if (erro?.response?.data?.errors)
                        alert(Object.values(erro.response.data.errors).flat())
                    else
                        alert("Ocorreu um erro ao comunicar com o servidor. Tente novamente mais tarde.")
                });
        } else {
            await api.criarTarefa(tarefa)
                .then(navigate('/'))
                .catch(erro => {
                    if (erro?.response?.data?.errors)
                        alert(Object.values(erro.response.data.errors).flat())
                    else
                        alert("Ocorreu um erro ao comunicar com o servidor. Tente novamente mais tarde.")
                });
        }
    };

    const formatarDataHoraInput = (data) => {
        if (!data)
            return ''

        const dataObject = new Date(data)
        const ano = dataObject.getFullYear();
        const mes = String(dataObject.getMonth() + 1).padStart(2, '0');
        const dia = String(dataObject.getDate()).padStart(2, '0');
        const horas = String(dataObject.getHours()).padStart(2, '0');
        const minutos = String(dataObject.getMinutes()).padStart(2, '0');
        return `${ano}-${mes}-${dia}T${horas}:${minutos}`;
    };

    return (
        <div>
            <h1>{id ? 'Editar Tarefa' : 'Nova Tarefa'}</h1>
            <Form onSubmit={enviar}>
                <Form.Group>
                    <Form.Label>Título</Form.Label>
                    <Form.Control type="text" value={tarefa.titulo} onChange={(e) => setTarefa({ ...tarefa, titulo: e.target.value })} />
                </Form.Group>
                <Form.Group>
                    <Form.Label>Descrição</Form.Label>
                    <Form.Control as="textarea" rows={3} value={tarefa.descricao} onChange={(e) => setTarefa({ ...tarefa, descricao: e.target.value })} />
                </Form.Group>
                <Form.Group>
                    <Form.Label>Status</Form.Label>
                    <Form.Control disabled={!id} as="select" value={tarefa.status} onChange={(e) => setTarefa({ ...tarefa, status: e.target.value })}>
                        <option value="Pendente">Pendente</option>
                        <option value="EmProgresso">Em Progresso</option>
                        <option value="Concluida">Concluída</option>
                    </Form.Control>
                </Form.Group>
                <Form.Group>
                    <Form.Label>Data de conclusão</Form.Label>
                    <Form.Control disabled={!id} type="datetime-local" value={formatarDataHoraInput(tarefa.dataConclusao)} onChange={(e) => setTarefa({ ...tarefa, dataConclusao: e.target.value })} />
                </Form.Group>
                <div className='d-flex justify-content-center gap-2 mt-2'>
                    <Button variant="primary" type="submit">Salvar</Button>
                    <Link to='/' className="btn btn-danger ">Cancelar</Link>
                </div>
            </Form>
        </div>
    );
};

export default FormularioTarefa;