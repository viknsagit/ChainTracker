# Chain Tracker

## Overview
**Rework of [old version](https://github.com/viknsagit/ethereum_indexer) with microservices**

Ethereum Blockchain Indexer is a .NET 8-based application designed to monitor and index activities on the Ethereum blockchain. It tracks new blocks, transactions, and contract interactions, storing relevant data in a database for analysis and querying.

### Features

- **Block Tracking**: Monitors new blocks as they are mined on the Ethereum network.
- **Transaction Indexing**: Records details of transactions, including sender, receiver, and status.

### Future Enhancements

- **Support for Additional Contract Types**: Expand capabilities to include monitoring and indexing of various smart contract types beyond ERC20 tokens.
- **Performance Optimization**: Integrate Redis for caching to enhance data retrieval speed and reduce database load.
- **Contract Validation**: Implement mechanisms to validate the integrity and correctness of smart contracts during interactions.
- **Database Flexibility**: Add support for multiple databases such as MySQL, PostgreSQL, and SQLite for improved deployment flexibility.
- **ERC20 Token Detection**: Identifies ERC20 tokens and tracks their activities on the blockchain.
- **Contract Monitoring**: Detects and analyzes interactions with Ethereum smart contracts.

## Getting Started

### Prerequisites

- .NET 8 SDK or higher
- Ethereum node or access to an Infura endpoint for Ethereum RPC communication
- Database server (PostgreSQL)
- Kafka
- Docker

### Installation

In development

## Contribution

Contributions to the project are welcome! If you encounter issues or have suggestions for improvements, please open an issue or submit a pull request on the GitHub repository: [ChainTracker issues](https://github.com/viknsagit/ChainTracker/issues)

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

This README provides a comprehensive overview of your Ethereum Blockchain Indexer project, including setup instructions, features, future enhancements, and guidelines for contribution. Adjustments were made to incorporate future plans for database flexibility as discussed.
