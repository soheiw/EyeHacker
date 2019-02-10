import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv('graph.csv', names=['num1', 'num2'])
plt.plot(df['num1'], df['num2'])
plt.show()