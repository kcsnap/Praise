> login
az login

**** Change PROD to TEST for test environment ****

> create resource group
new-azresourcegroup -Name Rg-Praise-Prod -location "UK South"

> execute the template to create the infrastructure
$templateFile = "C:\SnapDev\praise\IaC\template.json"
$paramsFile = "C:\SnapDev\praise\IaC\prod.parameters.json"
New-AzResourceGroupDeployment -Name praisetemplate -ResourceGroupName Rg-Praise-Prod -TemplateFile $templateFile -TemplateParameterFile $paramsFile