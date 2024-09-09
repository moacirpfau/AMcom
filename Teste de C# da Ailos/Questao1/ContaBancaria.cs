using System;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public int Numero { get; private set; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }

        private const double TaxaSaque = 3.50;

        public ContaBancaria(int numero, string titular, double depositoInicial = 0)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0;
        }

        public void Deposito(double valor)
        {
            if (valor > 0)
            {
                Saldo = Saldo + valor;
            }
            else
            {
                throw new ArgumentException("O valor do depósito deve ser positivo.");
            }
        }

        public void Saque(double valor)
        {
            if (valor > 0)
            {
                Saldo = Saldo - (valor + TaxaSaque);
            }
            else
            {
                throw new ArgumentException("O valor do saque deve ser positivo.");
            }
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}
