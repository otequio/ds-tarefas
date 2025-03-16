const opcoes = {
  day: '2-digit', // Dia com 2 dígitos
  month: '2-digit', // Mês com 2 dígitos
  year: 'numeric', // Ano com 4 dígitos
  hour: '2-digit', // Hora com 2 dígitos
  minute: '2-digit', // Minutos com 2 dígitos
  hour12: false // Usar formato 24 horas
};

const formatarData = (data) => {
  if (!data) return '';
  const dataFormatar = new Date(data);
  return dataFormatar.toLocaleDateString('pt-BR', opcoes)
}

export default formatarData