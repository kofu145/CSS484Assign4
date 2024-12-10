import os
import torch
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from torch.utils.data import Dataset, DataLoader
from sklearn import preprocessing
from torch import nn
from torch.nn import functional as F
from enum import Enum 
from pytorch_lightning import LightningModule, Trainer
from torchmetrics import Accuracy
import copy

df = pd.read_csv("export.csv")

class DatasetType(Enum):
    TRAIN = 1
    TEST = 2
    VAL = 3

class CustomDataset(Dataset):
    def __init__(self):
        self.df=pd.read_csv("export.csv").sample(frac=1, random_state=27)
        train_split=0.6
        val_split=0.8
        self.df_labels=df['label']
        self.df=df.drop(columns=['label','file_name'])
        self.train, self.val, self.test = np.split(self.df, [int(train_split*len(self.df)), int(val_split*len(self.df))])
        self.train_labels, self.val_labels, self.test_labels = np.split(self.df_labels, [int(train_split*len(self.df)), int(val_split*len(self.df))])
        self.scaler=preprocessing.StandardScaler().fit(self.train)
        for data_split in [ self.train, self.val, self.test]:
            data_split=self.scaler.transform(data_split)

            
    def __len__(self):
        return len(self.dataset)

    def __getitem__(self, idx):
        return self.dataset[idx],self.labels[idx]
    
    def set_fold(self,set_type):
        if set_type==DatasetType.TRAIN:
            self.dataset,self.labels=self.train,self.train_labels
        if set_type==DatasetType.TEST:
            self.dataset,self.labels=self.test,self.test_labels
        if set_type==DatasetType.VAL:
            self.dataset,self.labels=self.val,self.val_labels


        self.dataset=torch.tensor(self.scaler.transform(self.dataset)).float()
        self.labels=torch.tensor(self.labels.to_numpy().reshape(-1)).long()

        return self
        
dataset=CustomDataset()
train=copy.deepcopy(dataset).set_fold(DatasetType.TRAIN)
test=copy.deepcopy(dataset).set_fold(DatasetType.TEST)
val=copy.deepcopy(dataset).set_fold(DatasetType.VAL)
for i in [train,test,val]:
    print(i.__getitem__(0)[1])



BATCH_SIZE=4

class SimpleModel(LightningModule):
    def __init__(self,train,test,val):
        super().__init__()
        self.train_ds=train
        self.val_ds=val
        self.test_ds=test

        # two classes for each label: yes/no for our music/speech
        classes=2
        features=4
        self.model = nn.Sequential(
            nn.Flatten(),
            nn.Linear(features, 128),
            nn.ReLU(),
            nn.Dropout(0.1),
            nn.Linear(128, 32),
            nn.ReLU(),
            nn.Dropout(0.1),
            nn.Linear(32, classes),
        )
        self.accuracy = Accuracy(task='multiclass', num_classes=2)
    def forward(self, x):
        x = self.model(x)
        return F.log_softmax(x, dim=1)
    
    def training_step(self, batch, batch_idx):
        x, y = batch
        logits = self(x)
        loss = F.nll_loss(logits, y)
        
        return loss
    
    def validation_step(self, batch, batch_idx, print_str="val"):
        x, y = batch
        logits = self(x)
        loss = F.nll_loss(logits, y)
        preds = torch.argmax(logits, dim=1)
        self.accuracy(preds, y)

        self.log(f"{print_str}_loss", loss, prog_bar=True)
        self.log(f"{print_str}_acc", self.accuracy, prog_bar=True)
        return loss
    
    def test_step(self, batch, batch_idx):
        # Here we just reuse the validation_step for testing
        return self.validation_step(batch, batch_idx,print_str='test')
    
    def configure_optimizers(self):
        return torch.optim.Adam(self.parameters(), lr=0.001)
    #
    # HERE: We define the 3 Dataloaders, only train needs to be shuffled
    # This will then directly be usable with Pytorch Lightning to make a super quick model
    def train_dataloader(self):
        return DataLoader(self.train_ds, batch_size=BATCH_SIZE,shuffle=True)

    def val_dataloader(self):
        return DataLoader(self.val_ds, batch_size=BATCH_SIZE,shuffle=False)

    def test_dataloader(self):
        return DataLoader(self.test_ds, batch_size=BATCH_SIZE,shuffle=False)
    
model=SimpleModel(train,test,val)
model.load_state_dict(torch.load("./testmodel.pt"))
model.eval()

my_df = pd.read_csv("./export.csv")
test = torch.tensor(my_df.drop(columns=["file_name", "label"]).values)
y_res = torch.LongTensor(my_df["label"])

correct = 0
with torch.no_grad():
    for i, data in enumerate(test):
        y_val = model(data)

        # type of speech/music
        print(f"{i+1}.) {str(y_val)}      {y_res[i]} \t {y_val.argmax().item()}")

        if y_val.argmax().item() == y_res[i]:
            correct += 1
print(correct)