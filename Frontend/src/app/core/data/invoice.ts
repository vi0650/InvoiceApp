const invoice = {
  invoiceNo: 'INV-0001',
  serialNumber: '0001',
  date: '06/02/2026',
  currency: 'USD',

  billedBy: {
    name: 'Invoicey Ltd',
    address: '122 Main St, AnyTown, USA',
    phone: '+91 6546'
  },

  billedTo: {
    name: 'John Doe',
    address: '456 Second St, AnyTown, USA'
  },

  items: [
    { name: 'abc', description: 'wqe', qty: 100, price: 200 }
  ],

  taxPercent: 0
};
