import pandas as pd
import matplotlib.pyplot as plt
import math

f_sqrt = lambda x: math.sqrt(x)

df = pd.read_csv('wholeRisk101629.csv', names=['num1', 'risk', 'rotation'])
#plt.plot(df['num1'], df['risk'].apply(f_sqrt))
plt.plot(df['num1'], df['risk'])
plt.plot(df['num1'], df['rotation'])
plt.show()